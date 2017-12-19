﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Common.Persistence.Non_Relational;

namespace Kooboo.CMS.Content.Services
{
    /// <summary>
    /// 
    /// </summary>
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(MediaFolderManager))]
    public class MediaFolderManager : FolderManager<MediaFolder>
    {
        public MediaFolderManager(IMediaFolderProvider provider)
            : base(provider) { }

        public virtual void Rename(MediaFolder @new, MediaFolder old)
        {
            if (string.IsNullOrEmpty(@new.Name))
            {
                throw new NameIsReqiredException();
            }
            ((IMediaFolderProvider)Provider).Rename(@new, @old);
        }

        public override IEnumerable<FolderTreeNode<MediaFolder>> FolderTrees(Repository repository)
        {
            return All(repository, "").Where(it => ((MediaFolder)(object)it).AsActual() != null).Select(it => GetFolderTreeNode(it));
        }

        protected override FolderTreeNode<MediaFolder> GetFolderTreeNode(MediaFolder folder)
        {
            FolderTreeNode<MediaFolder> treeNode = new FolderTreeNode<MediaFolder>() { Folder = folder };
            treeNode.Children = ChildFolders(folder)
                .Where(it => it is MediaFolder)
                .Select(it => GetFolderTreeNode(it));
            return treeNode;
        }
    }
}
