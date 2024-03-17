import {
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableRow,
} from "@mui/material";
import { useStoreContext } from "../../context/StoreContext";

export default function BasketSummary() {
  const { basket } = useStoreContext();
  const subtotal =
    basket?.items.reduce((sum, item) => sum + item.quantity * item.price, 0) ??
    0;
  const deliveryFee = subtotal > 10000 ? 0 : 500;

  return (
    <TableContainer component={Paper} variant="outlined">
      <Table>
        <TableBody>
          <TableRow>
            <TableCell colSpan={2}>Subtotal</TableCell>
            <TableCell align="right">{subtotal}</TableCell>
          </TableRow>
          <TableRow>
            <TableCell colSpan={2}>Delivery Fee*</TableCell>
            <TableCell align="right">{deliveryFee}</TableCell>
          </TableRow>
          <TableRow>
            <TableCell colSpan={2}>Total</TableCell>
            <TableCell align="right">{subtotal + deliveryFee}</TableCell>
          </TableRow>
          <TableRow>
            <TableCell>
              <span style={{ fontStyle: "italic" }}>
                *Orders over $100 qualify for free delivery fee
              </span>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </TableContainer>
  );
}
