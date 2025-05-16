import { Product } from "./product.model";

export class Order {
    id: string;
    customerId: string;
    products: [Product];
    date: Date;
    totalPrice: number;
}