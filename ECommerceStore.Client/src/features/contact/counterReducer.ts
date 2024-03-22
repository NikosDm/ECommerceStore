import { initialState } from "./counterSlice";

export const INCREMENT_COUNTER = "INCREMENT_COUNTER";
export const DECREMENT_COUNTER = "DECREMENT_COUNTER";

interface CounterAction {
  type: string;
  payload: number;
}

export function increment(amount = 1) {
  return { type: INCREMENT_COUNTER, payload: amount };
}

export function decrement(amount = 1) {
  return { type: DECREMENT_COUNTER, payload: amount };
}

export default function counterReducer(
  state = initialState,
  action: CounterAction
) {
  switch (action.type) {
    case INCREMENT_COUNTER:
      // This return a new state. It does not mutate it.
      return {
        ...state,
        data: state.data + action.payload,
      };
    case DECREMENT_COUNTER:
      // This return a new state. It does not mutate it.
      return {
        ...state,
        data: state.data - action.payload,
      };
    default:
      return state;
  }
}
