import { Product } from "./product.model";

export class Order {
    _id: string;
    customerId: string;
    products: [Product];
    date: Date;
    totalPrice: number;
}