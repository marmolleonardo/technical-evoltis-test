import { createFeatureSelector, createSelector } from '@ngrx/store';
import { productsFeatureKey, ProductState, adapter } from './product.reducer';

export const selectProductsState = createFeatureSelector<ProductState>(productsFeatureKey);

const { selectAll, selectEntities, selectIds, selectTotal } = adapter.getSelectors();

export const selectAllProducts = createSelector(selectProductsState, selectAll);
export const selectProductsEntities = createSelector(selectProductsState, selectEntities);
export const selectProductsCount = createSelector(selectProductsState, selectTotal);
export const selectProductsLoading = createSelector(selectProductsState, (s) => s.loading);
export const selectProductsError = createSelector(selectProductsState, (s) => s.error);

export const selectProductById = (id: string) =>
  createSelector(selectProductsEntities, (entities) => entities[id] ?? null);
