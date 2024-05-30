
using Dto;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using PublicCommon;
using System.Net.Http;
using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using BlazorWebApp.Shared;
using Microsoft.JSInterop;
using System.Data;
using System.Net;
using MudBlazor;
using BaseBlazorComponentsRCL;
using Microsoft.AspNetCore.Components.Authorization;
using PublicCommon.MyVote;

namespace BlazorRazorClassLibraryMyVote
{
    public partial class ConstituencySearchAndResult : ComponentBase
        {
        [Parameter]
        public string SearchTerm { get; set; } = string.Empty;

        [Parameter]
        public bool? IsToAdd { get; set; }

        [Parameter]
        public int VoteId { get; set; }
        [Parameter]
        public bool IsSupportOppose { get; set; }

        [Parameter]
        public EventCallback<bool> HideHeaderAndFooter { get; set; }

        [Inject] AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private MudAutocomplete<string> autoComplete;

        //[Inject] IConfigurationService configService { get; set; }//if necessary then call

        //private string Name { get; set; } = "Member24.in";//change this & refer from configs
        private List<ConstituencyDto> _constituencies = [];//on default load last/recent/top search results
        private List<ConstituencyDto> _allConstituencies = [];//stays offline
        private VoteDto? _myVote = null;
        private int? _myConstituencyIdAddedJustNow;//only for display purpose at the time of adding vote & refresh time
        private Guid? UserId { get; set; }

        private Timer timer;

        readonly int searchResultMaxCount = 7;
        //bool NoMatchFound = false;
        private List<string> recentSearches = [];
        bool addMyVoteConstituencyOnTopOfDefaultResult = false;

        public bool? FirstTime;
        private async Task<Guid?> GetUserIdFromClaim()
            {
            //await Http.LoadSettings();//this is not necessary tomload,bcz already application loaded on start
            //if any issue check back here
            var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
            var userPrincipal = authState.User;
            if (userPrincipal.Identity != null && userPrincipal.Identity.IsAuthenticated)
                {
                var userid = userPrincipal.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
                if (!string.IsNullOrEmpty(userid) && Guid.TryParse(userid, out Guid result))
                    { //return await UserDbContext.Users.FirstOrDefaultAsync(u => u.Id == userid); //returns IdentityUser
                    UserId = result;
                    return result;
                    }
                }
            UserId = null;
            return null;
            }

        public DateTime LastLoadedTime;// = DateTime.Now;
        protected override async Task OnInitializedAsync()
            {
            FirstTime = true;
            await GetUserIdFromClaim();

            await LoadData();
            if (_allConstituencies.HasData())
                {
                await MyVoteAdjustments();

                await Search(SearchTerm);
                if ((IsToAdd == true && int.TryParse(SearchTerm, out int cid) && _allConstituencies.Exists(x => x.Id == cid)) || (VoteId > 0 && IsSupportOppose != null))
                    {
                    await HideHeaderAndFooter.InvokeAsync(true);
                    }
                //timer = new Timer(600000,); // 10 minutes
                //timer.Elapsed += async (sender, e) => await LoadData();
                //timer.Start();
                FirstTime = false;
                }
            }
        protected override async Task OnAfterRenderAsync(bool firstRender)
            {
            if (firstRender)
                {
                await Task.Delay(100); // Introduce a small delay for making content to load
                if (autoComplete != null)
                    await autoComplete.FocusAsync();
                }
            await JSRuntime.InvokeVoidAsync("startFetchInterval", DotNetObjectReference.Create(this));
            await base.OnAfterRenderAsync(firstRender);
            }

        private async Task MyVoteAdjustments()
            {
            if (UserId != null)
                {
                // _myVote = await Http.GetFromJsonAsync<VoteDto?>("api/Vote/" + UserId.ToString());//this fails in case of 1st time no value

                _myVote = await Http.MyVoteGet(UserId);
                if (_myVote != null)
                    {
                    _myVote.KPIRatingMessages.LoadMessages(_myVote.KPIMessages);
                    var match = _allConstituencies.Find(x => x.Id == _myVote.ConstituencyId);
                    _myVote.ConstituencyName = match!.Name;
                    _myVote.State = match.State;
                    _myVote.CurrentMemberName = match.CurrentMemberName;
                    _myVote.CurrentMemberParty = match.CurrentMemberParty;
                    _myVote.CurrentMemberTerms = match.CurrentMemberTerms;
                    _myVote.ConstituencyRatingByOverAll = match.Summary?.Rating;
                    addMyVoteConstituencyOnTopOfDefaultResult = true;
                    }

                if (int.TryParse(SearchTerm, out int constId) && _allConstituencies.Exists(x => x.Id == constId))
                    {
                    if (IsToAdd == true &&
                        !(_myVote != null && _myVote.ConstituencyId == constId && IsRecentlyVoted(_myVote.Created, _myVote.LastModified)))
                        {
                        await OnAddUpdateMyVoteToConstituency(constId);
                        await HideHeaderAndFooter.InvokeAsync(true);
                        }
                    else if (VoteId > 0)
                        {
                        await SupportOppose(constId, VoteId, IsSupportOppose);
                        }
                    }

                }
            }

        public static bool IsRecentlyVoted(DateTime createdDate, DateTime? modifiedDate)
            {
            // Check if the difference is less than 5 minutes
            return ((DateTime.Now - (modifiedDate != null ? modifiedDate.Value : createdDate)).TotalMinutes < 5);
            }

        [JSInvokable]
        public async Task LoadData()//auto refresh enabled for this
            {
            if (DateTime.Now.Subtract(LastLoadedTime).TotalMinutes > 1)
                {
                var result = await Http.ConstituencyGetAll();
                if (result.IsEmpty())
                    {
                    Console.WriteLine($"Please try after Sometime(at {DateTime.Now.AddMinutes(10)}) & make sure internet conection");
                    }
                else
                    {
                    _allConstituencies = result!;
                    if (LastLoadedTime != default)
                        StateHasChanged(); // Notify Blazor the state has changed & a re-render is necessary
                    LastLoadedTime = DateTime.Now;
                    }
                }
            }
        private async Task<IEnumerable<string>> Search(string? value = null)
            {
            SearchTerm = value?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(value))
                _constituencies = [.. _allConstituencies];
            else
                {
                if (FirstTime == false)
                    await HideHeaderAndFooter.InvokeAsync(true);

                AddSearchToRecentSearchList(value);
                //todo later hanlde search of csv values to load favuorites searched results

                //case1.1: csv of numbers
                //case1.2: csv of strings
                //case1.3: csv of both string & numbers
                //case2: single number
                //case3: single name

                //if contains csv then go for split & further logic of addings to id,
                //else if trparse to int go for it
                //else consider as string

                _constituencies.Clear();

                if (value.Contains(','))
                    {
                    foreach (var token in value.Split(','))
                        {
                        var trimmedToken = token.Trim();
                        if (int.TryParse(trimmedToken, out var id))
                            {
                            if (_allConstituencies.Exists(c => c.Id == id))
                                _constituencies.Add(_allConstituencies.Find(c => c.Id == id));
                            }
                        else
                            {
                            _constituencies.AddRange(_allConstituencies.Where(c => c.Name.Contains(trimmedToken, StringComparison.OrdinalIgnoreCase))
                                .Take(searchResultMaxCount));
                            }
                        }
                    }
                else if (int.TryParse(value, out var constId) && _allConstituencies.Exists(x => x.Id == constId))
                    {
                    _constituencies = [_allConstituencies.Find(x => x.Id == constId)!];
                    }
                else if (_allConstituencies.Exists(x => x.Name.Contains(value, StringComparison.OrdinalIgnoreCase)))
                    {
                    _constituencies = _allConstituencies.Where(x => x.Name.Contains(value, StringComparison.OrdinalIgnoreCase))
                            .Take(searchResultMaxCount).ToList();
                    }

                if (_constituencies.Count == 0)
                    {
                    if (_allConstituencies.Exists(x => x.State.Contains(value, StringComparison.OrdinalIgnoreCase)
                    || (!string.IsNullOrEmpty(x.CurrentMemberName) && x.CurrentMemberName.Contains(value, StringComparison.OrdinalIgnoreCase))
                    ))
                        {
                        _constituencies = _allConstituencies.Where(x => x.State.Contains(value, StringComparison.OrdinalIgnoreCase)
                   || (!string.IsNullOrEmpty(x.CurrentMemberName) && x.CurrentMemberName.Contains(value, StringComparison.OrdinalIgnoreCase))
                   ).Take(searchResultMaxCount).ToList();
                        }
                    //NoMatchFound = true;
                    }
                }
            if (_constituencies.Count < searchResultMaxCount)
                _constituencies.AddRange(_allConstituencies.Except(_constituencies).Take(searchResultMaxCount - _constituencies.Count));

            if (addMyVoteConstituencyOnTopOfDefaultResult && _myVote != null)
                {
                _constituencies.MoveItemToTopById<ConstituencyDto>(_allConstituencies.Find(x => x.Id == _myVote.ConstituencyId), obj => obj.Id);
                addMyVoteConstituencyOnTopOfDefaultResult = false;
                }
            _constituencies = _constituencies.Take(searchResultMaxCount).ToList();
            _expandedDictionaryOfConstituencyRatingsOnSummaryPage.Clear();
            _constituencies.ForEach(item => _expandedDictionaryOfConstituencyRatingsOnSummaryPage.Add(item.Id, false));
            //StateHasChanged();
            //return constituencies.Select(x => x.Name).ToList();
            return [];//since directly results shown ,so not showing anything on textbox
            }

        //todonext
        private Dictionary<int, bool> _expandedDictionaryOfConstituencyRatingsOnSummaryPage = [];
        private void OnExpandCollapseClickOfConstituencyKpisOnSummaryPage(int constituencyId)
            {
            _expandedDictionaryOfConstituencyRatingsOnSummaryPage[constituencyId] = !_expandedDictionaryOfConstituencyRatingsOnSummaryPage[constituencyId];
            }
        private void ConstituencySummaryKpiDisplaySetFalseDefault(List<int> ids)
            {
            _expandedDictionaryOfConstituencyRatingsOnSummaryPage.Clear();
            ids.ForEach(id => _expandedDictionaryOfConstituencyRatingsOnSummaryPage.Add(id, false));
            }
        public void HandleVoteSubmitted(VoteUpdateResult result)
            {
            if (result.IsSuccess)
                {
                _myConstituencyIdAddedJustNow = result.ConstituencyId;
                _myVote = result.UpdatedVote;
                NavigationManager.NavigateTo($"{VoteConstants.ConstituencyPrefix}/{result.ConstituencyId}");
                }
            var constituency = _constituencies.Find(x => x.Id == result.ConstituencyId);
            if (constituency != null)
                {
                constituency.VoteOfLoggedInUser = null;
                constituency.VoteUpdateSuccess = result.IsSuccess;
                constituency.VoteUpdateResultMessage = result.ResultMessage;
                }
            }

        private async Task OnAddUpdateMyVoteToConstituency(int constituencyId)
            {
            if (UserId.HasValue && _allConstituencies.HasData())
                {
                await HideHeaderAndFooter.InvokeAsync(true);
                if (_constituencies.IsEmpty())
                    _constituencies = _allConstituencies.Take(searchResultMaxCount).ToList();

                var constituencyDto = _constituencies.Find(x => x.Id == constituencyId);
                if (constituencyDto == null)
                    {
                    if (_allConstituencies.Exists(x => x.Id == constituencyId))
                        {
                        constituencyDto = _allConstituencies.Find(x => x.Id == constituencyId);
                        _constituencies.Insert(0, constituencyDto!);
                        _expandedDictionaryOfConstituencyRatingsOnSummaryPage.Add(constituencyId, false);
                        constituencyDto = _constituencies[0];
                        }
                    else { return; }
                    }
                ConstituencySummaryKpiDisplaySetFalseDefault(_constituencies.Select(x => x.Id).ToList());
                constituencyDto.VoteOfLoggedInUser = (_myVote != null && _myVote.ConstituencyId == constituencyId ? _myVote.CloneBySerializing() : null)
                         ?? new VoteDto() { ConstituencyId = constituencyId, UserId = UserId };

                foreach (var item in _allConstituencies.Where(x => x.VoteOfLoggedInUser != null && x.Id != constituencyId))
                    {
                    //making all other if any has addvote page is opened
                    item.VoteOfLoggedInUser = null;
                    }
                //StateHasChanged();

                }
            else
                {
                NavigationManager.NavigateTo($"Account/Login?returnUrl={Uri.EscapeDataString($"{VoteConstants.AddVotePrefix}/{constituencyId}/true")}");
                //after login opens particular constituencyId as search term, & opens addvote view
                }
            }

        private async Task GetMessages(int constituencyId)
            {
            //actually on page load now messages are loading top 30, so this option has to appear only for more than that remains
            //but the probllem is how many to keep on memory
            //it can be infinite right?
            //had to test .......

            //todo not necessarily everytime had to call server,instead check for it locally & then return. because earlier if already loaded then no need again.some more logic required 
            //probably for every batch it can be loaded
            ////////var match = _allConstituencies.Find(x => x.Id == constituencyId);
            ////////if (match != null)
            ////////    {
            ////////    var result = await Mediator.Send(new GetByConstituencyIdQuery() { ConstituencyId = constituencyId, Count = match.Summary?.MessagesCount, IncludeNonMessagesAlso = false, ViewerUserId = UserId }).ConfigureAwait(false);//fetches all
            ////////    if (result.HasData())
            ////////        {
            ////////        if (match.Votes.IsEmpty())
            ////////            match.Votes ??= result;
            ////////        else
            ////////            match.Votes!.AddRange(result);
            ////////        }
            ////////    //extracts messages,supportcount,opposecount,
            ////////    }
            }
        private async Task SupportOppose(int constituencyId, int voteId, bool? support)
            {
            var match = _allConstituencies.Find(x => x.Id == constituencyId);
            if (match != null)
                {
                if (UserId.HasValue)
                    {
                    VoteDto? mySupportForThisVote = null;
                    if (match.Votes.HasData())
                        {
                        mySupportForThisVote = match.Votes!.Find(x => x.Id == voteId);
                        if (mySupportForThisVote != null)
                            {
                            if (mySupportForThisVote.MySupportAsAViewer == support)
                                {
                                //support = !support;//here we can make it as a negation like support
                                support = null;//this to make un-support by which it makes no-support
                                               //even if its same we can ignore also 
                                }
                            }
                        }
                    //  else mySupportForThisVote = null;
                    var result = await Http.VoteSupportOpposePost(constituencyId, voteId, UserId, support);
                    //var result = await Http.PostAsJsonAsync(ApiEndPoints.VoteSupportOpposePost, new VoteSupportOppose(constituencyId, voteId, UserId ?? new Guid(), support));
                    if (result > 0 && mySupportForThisVote != null && mySupportForThisVote.MySupportAsAViewer != support)//means success
                        {
                        if (support == true)
                            {
                            if (mySupportForThisVote.MySupportAsAViewer == false)
                                mySupportForThisVote.OpposeCount--;
                            mySupportForThisVote.SupportCount++;
                            }
                        else if (support == false)
                            {
                            if (mySupportForThisVote.MySupportAsAViewer == true)
                                mySupportForThisVote.SupportCount--;
                            mySupportForThisVote.OpposeCount++;
                            }
                        else
                            {
                            if (mySupportForThisVote.MySupportAsAViewer == false)
                                mySupportForThisVote.OpposeCount--;
                            if (mySupportForThisVote.MySupportAsAViewer == true)
                                mySupportForThisVote.SupportCount--;
                            }
                        mySupportForThisVote.MySupportAsAViewer = support;

                        }
                    //extracts messages,supportcount,opposecount,
                    }
                else
                    {
                    NavigationManager.NavigateTo($"Account/Login?returnUrl={Uri.EscapeDataString($"{VoteConstants.UpVotePrefix}/{constituencyId}/{voteId}/{support}")}");
                    }
                }
            }

        private List<string> GetRecentSearchList()
            {
            var searches = new List<string>(recentSearches);
            // Add the voted ID if it's not already in the list
            if (_myVote != null && !recentSearches.Contains(_myVote.ConstituencyName))
                {
                searches.Add(_myVote.ConstituencyName);
                }
            return searches.Take(searchResultMaxCount).ToList();
            }
        private void AddSearchToRecentSearchList(string search)
            {
            if (string.IsNullOrEmpty(search)) return;
            //// Remove the search if it already exists to avoid duplicates
            //recentSearches.Remove(search);
            // Remove any existing searches that are contained within the new search
            recentSearches.RemoveAll(s => search.Contains(s, StringComparison.InvariantCultureIgnoreCase));

            // Add the search to the start of the list
            recentSearches.Insert(0, search);

            // Limit the list to the last 5 searches
            if (recentSearches.Count > searchResultMaxCount)
                {
                recentSearches = recentSearches.Take(searchResultMaxCount).ToList();
                }
            }

        public void Dispose()
            {
            timer?.Dispose();
            }
        }
    }
