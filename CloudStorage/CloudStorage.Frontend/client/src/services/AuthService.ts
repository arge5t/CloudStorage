import $api from "../http";
import { AxiosResponse } from "axios"
import { ILoginResponse } from "../models/response/AuthResponse";

export default class AuthService {
  static async login(email: string, password: string): Promise<AxiosResponse<ILoginResponse>> {
    return $api.post<ILoginResponse>(
      "/Account/login",
      { email, password });
  }

  static async registration(name: string, email: string, password: string):   Promise<AxiosResponse<string>> {
    return $api.post<string>(
      "/Accout/registration",
      { name, email, password });
  }

  static async logout():
    Promise<void> {
    return $api.post("/Accout/logout");
  }
}