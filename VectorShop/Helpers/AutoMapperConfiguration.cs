using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using VectorShop.Models;
using VectorShop.Models.ViewModels;

namespace VectorShop.Helpers
{
    public static class AutoMapperConfiguration
    {
        public static void Configure()
        {
            ConfigureProductMapping();
            ConfigureSlideShowMapping();
            ConfigureAdvertMapping();
            ConfigureArticleMapping();
            ConfigureContactMapping();
            ConfigureLinkMapping();
            ConfigureNewDesignOrderMapping();
            ConfigureOrderMapping();
            ConfigurePriCatMapping();
            ConfigureSecCatMapping();
            ConfigureControlPanelMapping();
            ConfigureNewsLetterMapping();
        }
        private static void ConfigureProductMapping()
        {
            Mapper.CreateMap<Product, ProductViewModel>();
        }
        private static void ConfigureAdvertMapping()
        {
            Mapper.CreateMap<Advert, AdvertViewModel>();
        }
        private static void ConfigureArticleMapping()
        {
            Mapper.CreateMap<Article, ArticleViewModel>();
        }
        private static void ConfigureContactMapping()
        {
            Mapper.CreateMap<Contact, ContactViewModel>();
        }
        private static void ConfigureLinkMapping()
        {
            Mapper.CreateMap<Link, LinkViewModel>();
        }
        private static void ConfigureNewDesignOrderMapping()
        {
            Mapper.CreateMap<NewDesignOrder, NewDesignOrderViewModel>();
        }
        private static void ConfigureOrderMapping()
        {
            Mapper.CreateMap<Order, OrderViewModel>();
        }
        private static void ConfigurePriCatMapping()
        {
            Mapper.CreateMap<PriCat, PriCatViewModel>();
        }
        private static void ConfigureSecCatMapping()
        {
            Mapper.CreateMap<SecCat, SecCatViewModel>();
        }
        private static void ConfigureSlideShowMapping()
        {
            Mapper.CreateMap<SlideShow, SlideShowViewModel>();
        }
        private static void ConfigureControlPanelMapping()
        {
            Mapper.CreateMap<ControlPanel, ControlPanelViewModel>();
        }
        private static void ConfigureNewsLetterMapping()
        {
            Mapper.CreateMap<NewsLetter, NewsLetterViewModel>();
        }
    }
}