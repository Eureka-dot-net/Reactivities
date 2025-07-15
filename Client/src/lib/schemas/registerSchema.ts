import z from "zod";
import { requiredString } from "../util/util";

export const registerSchema = z.object({
    email: z.string().email(),
    displayName: requiredString('Display name'),
    password: requiredString('Password')
})

export type RegisterSchema = z.infer<typeof registerSchema>