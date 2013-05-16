using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thesis.Common.Interfaces;

namespace Shell
{
    public interface IFilesRepository: IDetailRepository<FileViewModel>
    {
        bool Delete(int id);
    }
}
