using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MvcSiteMapProvider;
using VectorShop.Models;

namespace VectorShop.Helpers
{
    public class DynamicNodeProviders : DynamicNodeProviderBase
    {
        public override IEnumerable<DynamicNode> GetDynamicNodeCollection(ISiteMapNode node)
        {
            var nodes = new List<DynamicNode>();

            using (var _db = new VectorShopDb())
            {


                // Create a node for each Product 
                foreach (var product in _db.Products)
                {
                    DynamicNode productNode = new DynamicNode();
                    productNode.Title = product.ProductName;
                    productNode.UpdatePriority = UpdatePriority.Critical;
                    productNode.Controller = "Product";
                    productNode.Action = "Details";
                    productNode.ParentKey = "Home";
                    productNode.RouteValues.Add(
                        "id", product.ProductId.ToString() + "/" + VectorShopUtility.StringDasher(product.ProductName));

                    nodes.Add(productNode);
                }


                // Create a node for each Article 
                foreach (var article in _db.Articles)
                {
                    DynamicNode articleNode = new DynamicNode();
                    articleNode.Title = article.ArticleTitle;
                    articleNode.UpdatePriority = UpdatePriority.Normal;
                    articleNode.Controller = "Article";
                    articleNode.Action = "Details";
                    articleNode.ParentKey = "Home";
                    articleNode.RouteValues.Add(
                        "id", article.ArticleId.ToString() + "/" + VectorShopUtility.StringDasher(article.ArticleTitle));

                    nodes.Add(articleNode);
                }

                // Create a node for each PriCategory 
                foreach (var priCat in _db.PriCats.Where(p => p.Products.Any()))
                {
                    
                        DynamicNode priCatNode = new DynamicNode();
                        priCatNode.Title = priCat.PriCatTitle;
                        priCatNode.UpdatePriority = UpdatePriority.Normal;
                        priCatNode.Controller = "Product";
                        priCatNode.Action = "ProductsPriCat";
                        priCatNode.ParentKey = "Home";
                        priCatNode.RouteValues.Add(
                            "id", priCat.PriCatId.ToString() + "/" + VectorShopUtility.StringDasher(priCat.PriCatTitle));

                        nodes.Add(priCatNode);
                    
                }

                // Create a node for each SecCategory 
                foreach (var secCat in _db.SecCats.Where(p => p.Products.Any()))
                {

                    DynamicNode secCatNode = new DynamicNode();
                    secCatNode.Title = secCat.SecCatTitle;
                    secCatNode.UpdatePriority = UpdatePriority.Normal;
                    secCatNode.Controller = "Product";
                    secCatNode.Action = "ProductsSecCat";
                    secCatNode.ParentKey = "Home";
                    secCatNode.RouteValues.Add(
                        "id", secCat.SecCatId.ToString() + "/" + VectorShopUtility.StringDasher(secCat.SecCatTitle));

                    nodes.Add(secCatNode);

                }
            }


            return nodes;
        }
    }
}