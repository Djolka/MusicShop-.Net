import { OrderProduct } from "./orderProduct.model";

export class Order {
    id: string;
    customerId: string;
    orderProducts: OrderProduct[];
    date: Date;
    totalPrice: number;
    isVerified: boolean;
}