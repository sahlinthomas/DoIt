using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoIt.Models
{
    public class GroupedTodoItems : ObservableCollection<TodoItem>
    {
        public string Name { get; set; } 
    }
}
