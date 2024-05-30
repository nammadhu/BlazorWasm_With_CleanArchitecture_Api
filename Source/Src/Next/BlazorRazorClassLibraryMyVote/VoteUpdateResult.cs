using Dto;

namespace BlazorRazorClassLibraryMyVote
    {
    public class VoteUpdateResult(int ConstituencyId, bool IsSuccess, string? ResultMessage)
        {
        public int ConstituencyId { get; set; } = ConstituencyId;
        //public int VoteId { get; set; }
        public bool IsSuccess { get; set; } = IsSuccess;
        public string? ResultMessage { get; set; } = ResultMessage;
        public VoteDto? UpdatedVote { get; set; }
        }
    }
