export interface LoginRequestDto {
    username: string;
    password: string
}

export interface LoginResponseDto {
    token: string;
}

export interface RegisterRequestDto {
    username: string;
    password: string
}

export interface RegisterResponseDto {
    token: string;
}