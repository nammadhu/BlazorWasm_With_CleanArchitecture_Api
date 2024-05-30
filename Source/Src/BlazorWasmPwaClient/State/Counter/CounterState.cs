using Fluxor;

namespace BlazorWasmPwaClient.State.Counter
    {
    [FeatureState]
    public record CounterState
        {
        public int ClickCount { get; init; }
        }
    }
