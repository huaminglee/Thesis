using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Abstracts;
using Thesis.Common.Models;
using Thesis.Common.ViewModels;
using Thesis.Entities;
using System.Data.Objects;

namespace Crm
{
    public class RoofRepository : BaseRepository<ThesisObjectContext>, IRoofRepository
    {
        #region Compiled Queries
        /*
        static readonly Func<AxiomObjectContext, int, Roofs> cqGetById = CompiledQuery.Compile<AxiomObjectContext, int, Roofs>(
            (ctx, roofId) => ctx.Roofs.Where(ti => ti.RoofID == roofId).FirstOrDefault());
        */
        #endregion

        #region IListRepository Members

        public bool Delete(List<int> models)
        {
            return false;// DeleteEntities(models, cqGetById);
        }

        public IEnumerable GetByFilter(IDataViewModel viewModel, out int totalCount)
        {
            totalCount = 0;
            return null;
            /*
            var query = context.Roofs
                       .Select(a => new
                       {
                           RoofID = a.RoofID,
                           AddressID = a.AddressID,
                           Name = a.Name,
                           SurfaceAreaM2 = a.SurfaceAreaM2
                       });

            return query.ToList(viewModel, out totalCount);
            */
        }

        #endregion
    }
}
