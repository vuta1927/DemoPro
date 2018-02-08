export interface IUser{
    id: number;
    username: string;
    email: string;
    firtname: string;
    lastname: string;
    accessFailedCount: number;
    isLockoutEnabled: boolean;
    lockoutEndDateUtc: Date;
    isActive: boolean;
}