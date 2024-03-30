import * as yup from "yup";

export const validationSchema = [
  yup.object({
    fullName: yup.string().required("Full Name is required"),
    address1: yup.string().required("Address line 1 is required"),
    address2: yup.string().required("Address line 2 is required"),
    city: yup.string().required("City is required"),
    state: yup.string().required("State is required"),
    zip: yup.string().required("Zip Code is required"),
    country: yup.string().required("Country is required"),
  }),
  yup.object(),
  yup.object({
    nameOnCard: yup.string().required("Name on card is required"),
    cardNumber: yup
      .string()
      .required("Card number is required")
      .matches(/^[0-9]{4} [0-9]{4} [0-9]{4} [0-9]{4}$/, {
        message: "Card Number format is invalid",
      }),
    expDate: yup
      .string()
      .required("Expiry date is required")
      .matches(/^(0[1-9]|1[0-2])\/?(([0-9]{4}|[0-9]{2})$)/, {
        message: "Expiry date should be in format MM/YYYY",
      })
      .test("checkDate", "Expiry date is invalid", (val, ctx) => {
        const dateParts = val.split("/");
        const month = Number(dateParts[0]);

        if (month > 12) return ctx.createError({ message: "Month is invalid" });

        const now = new Date();

        if (
          now.getMonth() > month ||
          now.getFullYear() > Number(dateParts[1]) + 2000
        )
          return ctx.createError({
            message: "Expiry date should not be in the past",
          });

        return true;
      }),
    cvv: yup
      .string()
      .required("CVV is required")
      .matches(/^[0-9]{3}$/, {
        message: "CVV is invalid",
      }),
  }),
];
