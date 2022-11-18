using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.Domain.Models
{

  public class EditorEnvironment : IEditorEnvironment
  {
    public string GameId { get; set; }
    public string GameName { get; set; }
    public string GamePath { get; set; }
  }

}
