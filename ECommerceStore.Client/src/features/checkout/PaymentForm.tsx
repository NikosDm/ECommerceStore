import { Typography, Grid } from "@mui/material";
import { useFormContext } from "react-hook-form";
import AppTextInput from "../../app/components/AppTextInput";
import AppCheckbox from "../../app/components/AppCheckbox";

export default function PaymentForm() {
  const { control, formState } = useFormContext();
  return (
    <>
      <Typography variant="h6" gutterBottom>
        Payment method
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <AppTextInput
            name="nameOnCard"
            label="Name on card"
            control={control}
          />
        </Grid>
        <Grid item xs={12} md={6}>
          <AppTextInput
            name="cardNumber"
            label="Card number"
            control={control}
          />
        </Grid>
        <Grid item xs={12} md={6}>
          <AppTextInput name="expDate" label="Expiry date" control={control} />
        </Grid>
        <Grid item xs={12} md={6}>
          <AppTextInput name="cvv" label="CVV" control={control} />
        </Grid>
        <Grid item xs={12}>
          <AppCheckbox
            disabled={!formState.isDirty}
            name="saveCard"
            label="Remember credit card details for next time"
            control={control}
          />
        </Grid>
      </Grid>
    </>
  );
}
