import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IProductApi } from '../shared/models/productsApi';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  baseUrl = 'https://localhost:5001/api/';

  constructor(private http: HttpClient) {}

  // Method to add a new product
  addProduct(product: IProductApi): Observable<IProductApi> {
    return this.http.post<IProductApi>(`${this.baseUrl}products`, product);
  }

  deleteProduct(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}products/${id}`);
  }

  updateProductPrice(id: number, newPrice: number) {
    return this.http.put(`${this.baseUrl}products/${id}`, newPrice);
  }

  // Other methods like getProducts(), updateProduct(), deleteProduct() can also be included here.
}
