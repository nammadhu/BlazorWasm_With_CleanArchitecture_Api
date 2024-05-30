using Dto;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using PublicCommon;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using BlazorWebApp.Shared;
using BaseBlazorComponentsRCL;
using PublicCommon.MyVote;

namespace BlazorRazorClassLibraryMyVote
{
    public partial class AddUpdateVote : ComponentBase
        {

        [Parameter]
        public EventCallback<VoteUpdateResult> OnVoteSubmitted { get; set; }

        [Parameter]
        public required VoteDto Model { get; set; }

        [Parameter]
        public VoteDto? ExistingCurrentVote { get; set; }

        [Parameter]
        public required Guid UserId { get; set; }


        MudForm? _form;
        private bool _saving = false;
        private VoteDto _onPageLoadVoteState;

        private Dictionary<int, bool> _expandedDictionary = [];
        string? overWriteMessage;
        public bool Hide { get; set; } = false;
        protected override void OnInitialized()
            {
            _onPageLoadVoteState = Model.CloneBySerializing<VoteDto>();
            Model.KPIRatingMessages = KPI.MergeWithCurrentVoteKPIRatingMessageList(Model.KPIRatingMessages);

            // Initialize dictionary entries with initial expanded states (if needed)
            Model.KPIRatingMessages.ForEach(item => _expandedDictionary.Add(item.KPI_Id, false));

            if (ExistingCurrentVote != null && ExistingCurrentVote.ConstituencyId != Model.ConstituencyId)
                {
                overWriteMessage = $"This Removes Your Current Rating '{ExistingCurrentVote.Rating.RatingAsString().ToUpper()}' for {ExistingCurrentVote.ConstituencyName}/{ExistingCurrentVote.CurrentMemberName} Constituency dated ({(ExistingCurrentVote.LastModified ?? ExistingCurrentVote.Created)}.)";
                }
            Model.RatingForUiPurposeInNegativeRangeOnlyForDisplay = Model.KPIRatingMessages.CalculateRating();
            //return base.OnInitializedAsync();
            }
        private void OnReElectRadioChanged(bool? selectedOption)
            {
            Model.WorkDoneQuality = selectedOption;
            if (Model.KPIRatingMessages.IsEmpty())
                Model.KPIRatingMessages = KPI.MergeWithCurrentVoteKPIRatingMessageList(null);

            if (selectedOption == true)
                {
                //make all 4
                Model.KPIRatingMessages.Where(x => x.Rating == null || x.Rating == 0 || x.Rating == (int)RatingEnum.Bad).ToList().ForEach(x => x.Rating = (int)RatingEnum.GoodWork);

                }
            else if (selectedOption == false)
                {
                //make all 2
                Model.KPIRatingMessages.Where(x => x.Rating == null || x.Rating == 0 || x.Rating == (int)RatingEnum.GoodWork).ToList().ForEach(x => x.Rating = (int)RatingEnum.Bad);
                }
            else if (selectedOption == null)
                {
                //dont do anything ,leave all as it is
                }

            }

        public bool DataChanged { get; set; } = false;
        private void Undo()
            {
            if (_onPageLoadVoteState != null)
                {
                Model.KPIRatingMessages = KPI.MergeWithCurrentVoteKPIRatingMessageList(_onPageLoadVoteState.KPIRatingMessages).CloneBySerializing();
                Model.WorkDoneQuality = (bool?)_onPageLoadVoteState.WorkDoneQuality;
                StateHasChanged();
                }
            else
                {
                Clear();
                }
            DataChanged = false;
            }
        private void Clear()
            {
            Model.KPIRatingMessages = KPI.GetAllDefaultAsVoteKPIRatingMessageList();
            Model.WorkDoneQuality = null;
            DataChanged = true;
            //if (_onPageLoadVoteState != null && Model.ConstituencyId != _onPageLoadVoteState.Id)
            //    {
            //    //    _alertMessage = $"Removes Rating {Model.MyVoteCurrent.Rating.RatingAsString()} of {Model.MyVoteCurrent.ConstituencyName}/{Model.MyVoteCurrent.CurrentMemberName}";
            //    }
            StateHasChanged();
            }
        private void OnKpiNameClick(int itemId)
            {
            //if rating not selected select it
            var match = Model.KPIRatingMessages?.Find(x => x.KPI_Id == itemId);
            if (itemId != KPI.OpenIssuesKpiId && match != null && match.Rating == null)
                {
                match.Rating = (int)RatingEnum.OkOk;
                DataChanged = true;
                }
            else
                OnExpandCollapseClick(itemId);
            //if (_expandedDictionary.TryGetValue(itemId, out var value))
            //    _expandedDictionary[itemId] = !value;
            }
        private void OnExpandCollapseClick(int itemId)
            {
            if (itemId == KPI.OpenIssuesKpiId)

                _expandedDictionary[itemId] = !_expandedDictionary[itemId];
            else if (_expandedDictionary.TryGetValue(itemId, out var value))
                {
                var match = Model.KPIRatingMessages?.Find(x => x.KPI_Id == itemId);
                if (itemId != KPI.OpenIssuesKpiId && match != null && match.Rating > 0)
                    {
                    _expandedDictionary[itemId] = _expandedDictionary[itemId] != true;
                    }
                else
                    _expandedDictionary[itemId] = false;
                }


            if (_expandedDictionary.Count(x => x.Value) > 1)
                {
                var others = _expandedDictionary.Where(x => x.Key != itemId).Select(x => x.Key).ToList();
                foreach (var item in others)
                    {
                    _expandedDictionary[item] = false;
                    }
                }
            }

        private async Task Submit()
            {
            try
                {

                if (_onPageLoadVoteState.WorkDoneQuality == Model.WorkDoneQuality && _onPageLoadVoteState.KPIRatingMessages == Model.KPIRatingMessages.ValidRatings())
                    {
                    // Show a confirmation dialog or message indicating no changes
                    return;
                    }
                _saving = true;
                await _form!.Validate().ConfigureAwait(false);
                if (!_form!.IsValid)
                    return;

                // model.KPIRatingMessages = VoteHelperExtensions.RatingSetBackToOriginalRange(model.KPIRatingMessages);
                // var res = (await Http.GetFromJsonAsync<IEnumerable<ConstituencyDto>>("api/Constituency"))?.ToList();

                var result = await Http.VotePost(Model, UserId);

                //show loading symbol
                if (result > 0)
                    {
                    await OnVoteSubmitted.InvokeAsync(new VoteUpdateResult(Model.ConstituencyId, true, $"Vote Added Succesfully({Model.KPIRatingMessages.CalculateRating().RatingAsString()}),Summary will be updated soon!!!") { UpdatedVote = Model });
                    }
                else
                    {
                    await OnVoteSubmitted.InvokeAsync(new VoteUpdateResult(Model.ConstituencyId, false, $"Failed,Looks like some issue in Adding,please Re-Try after 10 minutes."));
                    }
                }
            catch (Exception e)
                {
                Console.WriteLine(e.ToString());
                }
            finally
                {
                _saving = false;
                }
            }
        /*
    private static VoteDto SetMemberDetailsToVoteDto(ConstituencyDto dto)
    {
    //todo we can add these using mappers
    return new VoteDto()
    {
    ConstituencyId = dto!.Id,
    ConstituencyName = dto.Name,
    State = dto.State,
    CurrentMemberName = dto.CurrentMemberName,
    CurrentMemberParty = dto.CurrentMemberParty,
    CurrentMemberTerms = dto.CurrentMemberTerms
    //,UserId = state//fetch from logged in account
                };
        }
    private static void SetMemberDetailsToVoteDto(VoteDto? voteDto, ConstituencyDto? constituencyDto)
    {
    //todo we can add these using mappers
    if (voteDto == null || constituencyDto == null || constituencyDto == default) voteDto = new VoteDto();
    voteDto.ConstituencyId = constituencyDto!.Id;
    voteDto.ConstituencyName = constituencyDto.Name;
    voteDto.State = constituencyDto.State;
    voteDto.CurrentMemberName = constituencyDto.CurrentMemberName;
    voteDto.CurrentMemberParty = constituencyDto.CurrentMemberParty;
    voteDto.CurrentMemberTerms = constituencyDto.CurrentMemberTerms;
    //,UserId = state//fetch from logged in account
    // return voteDto;
    } */
        }
    }
