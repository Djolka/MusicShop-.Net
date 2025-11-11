import { OrderProductDTO } from "./orderProduct-dto.model";

export class OrderDTO {
    customerId: string;
    orderProducts: OrderProductDTO[];
    date: Date;
    totalPrice: number;
}