import { createAction, props } from '@ngrx/store';
import { Product } from './product.models';
import { Update } from '@ngrx/entity';

const prefix = '[Products] ';

export const loadProducts = createAction(`${prefix}Load Products`);
export const loadProductsSuccess = createAction(
  `${prefix}Load Products Success`,
  props<{ products: Product[] }>()
);
export const loadProductsFailure = createAction(
  `${prefix}Load Products Failure`,
  props<{ error: string }>()
);

export const createProduct = createAction(
  `${prefix}Create Product`,
  props<{ product: Product }>()
);
export const createProductSuccess = createAction(
  `${prefix}Create Product Success`,
  props<{ product: Product }>()
);
export const createProductFailure = createAction(
  `${prefix}Create Product Failure`,
  props<{ error: string }>()
);

export const updateProduct = createAction(
  `${prefix}Update Product`,
  props<{ update: Update<Product> }>()
);
export const updateProductSuccess = createAction(
  `${prefix}Update Product Success`,
  props<{ update: Update<Product> }>()
);
export const updateProductFailure = createAction(
  `${prefix}Update Product Failure`,
  props<{ error: string }>()
);

export const deleteProduct = createAction(
  `${prefix}Delete Product`,
  props<{ id: string }>()
);
export const deleteProductSuccess = createAction(
  `${prefix}Delete Product Success`,
  props<{ id: string }>()
);
export const deleteProductFailure = createAction(
  `${prefix}Delete Product Failure`,
  props<{ error: string }>()
);
