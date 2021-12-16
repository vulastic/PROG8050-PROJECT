SELECT PromotionDetail.Id as 'Id', PromotionDetail.PromotionId as 'PromotionId', PromotionDetail.ProductId as 'ProductId', Product.CategoryId as 'Category', Product.Name as 'Name', Product.Price as 'Price', Product.Quantity as 'Quantity', PromotionDetail.Discount as 'Discount' FROM PromotionDetail INNER JOIN Product ON PromotionDetail.ProductId = Product.Id;