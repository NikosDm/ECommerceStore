import { TextField } from "@mui/material";
import { UseControllerProps, useController } from "react-hook-form";

interface AppTextInputProps extends UseControllerProps {
  label: string;
}

export default function AppTextInput(props: AppTextInputProps) {
  const { fieldState, field } = useController({ ...props, defaultValue: "" });

  return (
    <TextField
      {...props}
      {...field}
      fullWidth
      variant="outlined"
      error={!!fieldState.error}
      helperText={fieldState.error?.message}
    />
  );
}
