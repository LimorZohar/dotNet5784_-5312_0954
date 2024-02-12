using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PL;
    /// <summary>
    /// enum collection for engineer expertise
       
internal class LevelCollection : IEnumerable
{
    static readonly IEnumerable<BO.Expertise> s_enums =
(Enum.GetValues(typeof(BO.Expertise)) as IEnumerable<BO.Expertise>)!;

    public IEnumerator GetEnumerator() => s_enums.GetEnumerator();
}
