using DustMother.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DustMother.App.Pages
{
    /// <summary>
    /// Facade type since XAML can't do generic types. Implements <see cref="DetailsPage{StatisticsSave, StatisticsModel}"/>
    /// </summary>
    public abstract class StatisticsDetail : DetailsPage<StatisticsSave, StatisticsModel> { }
    /// <summary>
    /// Facade type since XAML can't do generic types. Implements <see cref="DetailsPage{ConquestSave, ConquestModel}"/>
    /// </summary>
    public abstract class ConquestBasePage : DetailsPage<ConquestSave, ConquestModel> { }
}
