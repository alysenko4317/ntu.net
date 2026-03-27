using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace webblog.ViewModels
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Заголовок обов'язковий")]
        [StringLength(100, ErrorMessage = "Довжина заголовка не може перевищувати 100 символів")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Дата публікації обов'язкова")]
        public DateTime PublishDate { get; set; }

        public bool IsDraft { get; set; }

        [Required(ErrorMessage = "Вміст поста не може бути порожнім")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Необхідно вибрати категорію")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
    }
}
