import { format, formatDistanceToNow, type DateArg } from "date-fns";
import z from "zod";

export function formatDate(date: DateArg<Date>)
{
    return format(date, 'dd MMM yyyy h:mm a');
}

export function timeGo(date: DateArg<Date>)
{
  return formatDistanceToNow(date) + ' ago'
}

export const requiredString = (fieldName: string) =>
    z.string({
      required_error: `${fieldName} is required`
    })
    .nonempty({ message: `${fieldName} is required` });