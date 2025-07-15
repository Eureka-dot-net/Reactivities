import {
  DateTimePicker,
  type DateTimePickerProps,
  LocalizationProvider
} from '@mui/x-date-pickers';
import { AdapterDateFns } from '@mui/x-date-pickers/AdapterDateFnsV3';
import {
  useController,
  type FieldValues,
  type UseControllerProps
} from 'react-hook-form';
import type {} from '@mui/x-date-pickers/AdapterDateFnsV3'; // load date types

type Props<T extends FieldValues> = UseControllerProps<T>
  & DateTimePickerProps<Date>;

export default function DateInput<T extends FieldValues>(props: Props<T>) {
  const { field, fieldState } = useController(props);

  return (
    <LocalizationProvider dateAdapter={AdapterDateFns}>
      <DateTimePicker
        {...props}
        {...field}
        value={field.value || null}
        onChange={field.onChange}
        slotProps={{
          textField: {
            fullWidth: true,
            error: !!fieldState.error,
            helperText: fieldState.error?.message
          }
        }}
      />
    </LocalizationProvider>
  );
}
