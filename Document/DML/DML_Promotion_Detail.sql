INSERT INTO PromotionDetail (PromotionId, ProductId, Discount) SELECT 1, Product.Id, 10 FROM Product WHERE Product.CategoryId = 1;