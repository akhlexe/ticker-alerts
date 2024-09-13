// export interface Result {
//   success: boolean;
//   errors: string[];
// }

export interface Result<T = null> {
  data: T | null;
  success: boolean;
  errors: string[];
}
