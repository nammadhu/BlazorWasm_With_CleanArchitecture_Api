using BaseBlazorComponentsRCL;
using BlazorRazorClassLibraryMyTown;
using CleanArchitecture.Domain.MyTown.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using MyTown.SharedModels.DTOs;
using PublicCommon;

namespace BlazorRazorClassLibraryMytown
    {
    public partial class AddUpdateTown : ComponentBase
        {

        [Parameter]
        public int TownId { get; set; }

        //[Parameter]
        public TownDto? ExistingCurrentTown { get; set; }

        [Parameter]
        public Guid UserId { get; set; }

        private TownDto Model;
        MudForm? _form;
        private bool _saving = false;
        private TownDto _onPageLoadTownState;
        [Inject] HttpClient http { get; set; }

        private Dictionary<int, bool> _expandedDictionary = [];
        string? overWriteMessage;
        public bool Hide { get; set; } = false;
        protected override async Task OnInitializedAsync()
            {
            if (TownId > 0)
                {
                ExistingCurrentTown = await http.TownGet(TownId, UserId);
                if (ExistingCurrentTown != null)
                    {
                    _onPageLoadTownState = ExistingCurrentTown.CloneBySerializing<TownDto>();
                    Model = ExistingCurrentTown.CloneBySerializing<TownDto>();
                    }
                }
            if (Model == null)
                Model = new TownDto();
            //return base.OnInitializedAsync();
            }
        protected override void OnInitialized()
            {

            // _onPageLoadTownState = Model.CloneBySerializing<Town>();
            //return base.OnInitializedAsync();
            }
        private async Task Submit()
            {
            try
                {
                var result = await http.TownPost(Model, UserId);

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
