import { Product } from "./product.model";

export class OrderProduct {
  productId: string;
  quantity: number;
  product: Product;
}
