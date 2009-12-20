using System;
using System.ComponentModel.DataAnnotations;
using Splits.Application;

namespace Bookstore.Application.Queries
{
  public class TestQuery : IQuery<string>
  {
    [Range(1, Int32.MaxValue)]
    [Required]
    public int? Bar { get; set; }
  }
}