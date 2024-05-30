using Fluxor;

namespace BlazorWasmPwaClient.State.Counter
    {
    public static class ReducerCounterAction
        {
        [ReducerMethod]
        public static CounterState ReduceIncrementCounterAction(CounterState currentState, IncrementCounterAction action)
            {
            return currentState with { ClickCount = currentState.ClickCount + 1 };
            //instead of creating new object separately easily modify existing & return using record feature

            //  return new() { ClickCount = currentState.ClickCount + 1 };
            }
        }
    }
