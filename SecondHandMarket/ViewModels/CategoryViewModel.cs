using SecondHandMarket.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SecondHandMarket.ViewModels
{
    public class IndexCategoryViewModel
    {
        public IndexCategoryViewModel(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            SubCategories = category.SubCategories.Select(c => new IndexCategoryViewModel(c)).ToList();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public List<IndexCategoryViewModel> SubCategories { get; set; }
    }
    public class AddCategoryViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// 父类别
        /// </summary>
        public int? ParentId { get; set; }
    }

    public class UpdateCategoryViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }
    }
}