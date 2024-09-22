// src/app/products-add/products-add.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from '../services/product.services';
import { IProductApi } from '../shared/models/productsApi';

@Component({
  selector: 'app-products-add',
  templateUrl: './products-add.component.html',
  styleUrls: ['./products-add.component.scss']
})
export class ProductsAddComponent implements OnInit {
  productForm: FormGroup;

  constructor(private fb: FormBuilder, private productService: ProductService) {}

  ngOnInit(): void {
    this.createProductForm();
  }

  createProductForm() {
    this.productForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3)]],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(1)]],
      pictureUrl: ['', Validators.required],
      productType: ["shoes", Validators.required],
      productBrand: ["nike", Validators.required],
    });
  }

  onSubmit() {
    if (this.productForm.valid) {
      const newProduct: IProductApi = {
        ...this.productForm.value,
        productTypeId: 1, // Placeholder, ideally fetched from server
        productBrandId: 1, // Placeholder, ideally fetched from server
      };

      this.productService.addProduct(newProduct).subscribe(response => {
        console.log('Product added successfully', response);
      }, error => {
        console.log('Error while adding product', error);
      });
    }
  }
}