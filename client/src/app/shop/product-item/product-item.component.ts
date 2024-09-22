import { Component, Input, OnInit } from '@angular/core';
import { IProduct } from 'src/app/shared/models/products';
import { ProductService } from '../../services/product.services'; // Using your ProductService for deletion

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.scss']
})
export class ProductItemComponent implements OnInit {
  @Input() product: IProduct;
  newPrice: number; // To hold the new price input

  constructor(private productService: ProductService) { }

  ngOnInit(): void {}

  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe(() => {
      console.log(`Product with ID ${id} deleted successfully`);
      // Perform any further UI actions like removing the product from view or refreshing the list
    }, error => {
      console.error('Error deleting product:', error);
    });
  }

  updatePrice() {
    if (this.newPrice !== undefined) {
      this.productService.updateProductPrice(this.product.id, this.newPrice).subscribe(() => {
        console.log(`Product price updated to ${this.newPrice}`);
        // Optionally, you can refresh the product details or update the price in the UI
        this.product.price = this.newPrice; // Update the displayed price
        this.newPrice = undefined; // Clear the input after update
      }, error => {
        console.error('Error updating product price:', error);
      });
    }
  }
}
