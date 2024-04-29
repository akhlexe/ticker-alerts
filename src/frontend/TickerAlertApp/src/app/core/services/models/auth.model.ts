export interface LoginRequestDto {
    username: string;
    password: string
}

export interface RegisterRequestDto {
    username: string;
    password: string
}

export interface AuthResponse {
    username: string;
    token: string;
    success: boolean;
    errorMessage: string;
}