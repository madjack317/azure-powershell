// 
// Copyright (c) Microsoft and contributors.  All rights reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// 
// See the License for the specific language governing permissions and
// limitations under the License.
// 

// Warning: This code was generated by a tool.
// 
// Changes to this file may cause incorrect behavior and will be lost if the
// code is regenerated.

using AutoMapper;
using Microsoft.Azure.Commands.ResourceManager.Common.Tags;
using Microsoft.Rest.Azure;
using Microsoft.Azure.Commands.Network.Models;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Management.Network.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using CNM = Microsoft.Azure.Commands.Network.Models;

namespace Microsoft.Azure.Commands.Network.Automation
{
    [Cmdlet(VerbsCommon.Get, "AzureRmApplicationSecurityGroup"), OutputType(typeof(PSApplicationSecurityGroup))]
    public partial class GetAzureRmApplicationSecurityGroup : NetworkBaseCmdlet
    {
        [Parameter(
            Mandatory = false,
            HelpMessage = "The resource group name of the application security group.",
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Alias("ResourceName")]
        [Parameter(
            Mandatory = false,
            HelpMessage = "The name of the application security group.",
            ValueFromPipelineByPropertyName = true)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        public override void Execute()
        {
            base.Execute();

            if(!string.IsNullOrEmpty(this.Name))
            {
                var vApplicationSecurityGroup = this.NetworkClient.NetworkManagementClient.ApplicationSecurityGroups.Get(ResourceGroupName, Name);
                var vApplicationSecurityGroupModel = Mapper.Map<CNM.PSApplicationSecurityGroup>(vApplicationSecurityGroup);
                vApplicationSecurityGroupModel.ResourceGroupName = this.ResourceGroupName;
                vApplicationSecurityGroupModel.Tag = TagsConversionHelper.CreateTagHashtable(vApplicationSecurityGroup.Tags);
                WriteObject(vApplicationSecurityGroupModel, true);
            }
            else
            {
                IPage<ApplicationSecurityGroup> vApplicationSecurityGroupPage;
                if(!string.IsNullOrEmpty(this.ResourceGroupName))
                {
                    vApplicationSecurityGroupPage = this.NetworkClient.NetworkManagementClient.ApplicationSecurityGroups.List(this.ResourceGroupName);
                }
                else
                {
                    vApplicationSecurityGroupPage = this.NetworkClient.NetworkManagementClient.ApplicationSecurityGroups.ListAll();
                }

                var vApplicationSecurityGroupList = ListNextLink<ApplicationSecurityGroup>.GetAllResourcesByPollingNextLink(vApplicationSecurityGroupPage,
                    this.NetworkClient.NetworkManagementClient.ApplicationSecurityGroups.ListNext);
                List<PSApplicationSecurityGroup> psApplicationSecurityGroupList = new List<PSApplicationSecurityGroup>();
                foreach (var vApplicationSecurityGroup in vApplicationSecurityGroupList)
                {
                    var vApplicationSecurityGroupModel = Mapper.Map<CNM.PSApplicationSecurityGroup>(vApplicationSecurityGroup);
                    vApplicationSecurityGroupModel.ResourceGroupName = NetworkBaseCmdlet.GetResourceGroup(vApplicationSecurityGroup.Id);
                    vApplicationSecurityGroupModel.Tag = TagsConversionHelper.CreateTagHashtable(vApplicationSecurityGroup.Tags);
                    psApplicationSecurityGroupList.Add(vApplicationSecurityGroupModel);
                }
                WriteObject(psApplicationSecurityGroupList);
            }
        }
    }
}
