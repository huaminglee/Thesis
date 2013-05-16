using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Entities;
using Thesis.Common.Abstracts;
using System.Data.Objects;
using System.Collections;
using Thesis.Common.ViewModels;
using Thesis.Common.Models;

namespace Shell
{
    public class FilesRepository : BaseRepository<ThesisObjectContext>, IFilesRepository
    {
        #region Compiled Queries

        static readonly Func<ThesisObjectContext, int, Files> cqGetById = CompiledQuery.Compile<ThesisObjectContext, int, Files>(
            (ctx, fileId) => ctx.Files.Where(ti => ti.FileID == fileId).FirstOrDefault());

        static readonly Func<ThesisObjectContext, int, FileViewModel> cqLoadViewModel = CompiledQuery.Compile<ThesisObjectContext, int, FileViewModel>(
            (ctx, fileId) => ctx.Files.Where(ti => ti.FileID == fileId).Select(p => new FileViewModel { 
                FileID = p.FileID,
                Alias = p.Alias,
                FileName = p.FileName,
                Mimetype = p.Mimetype,
                Path = p.Path,
                Size = p.Size                
            }).FirstOrDefault());

        #endregion

        #region IDetailRepository<FileViewModel> Members

        public FileViewModel LoadViewModel(int id)
        {
            return cqLoadViewModel.Invoke(context, id);
        }

        public bool Save(int id, FileViewModel viewModel, List<RuleViolation> validationResults)
        {
            return SaveViewModel(id, cqGetById, viewModel, validationResults);
        }

        public bool Delete(List<int> models)
        {
            return DeleteEntities(models, cqGetById);
        }

        #endregion

        #region IFilesRepository Members

        public bool Delete(int id)
        {
            return DeleteEntity(cqGetById.Invoke(context, id));
        }

        #endregion
    }
}
