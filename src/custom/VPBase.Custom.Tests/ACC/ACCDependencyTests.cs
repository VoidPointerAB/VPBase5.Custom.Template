using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using VPBase.Auth.Client.Code.ApiClients;
using VPBase.Auth.Client.Code.ApiClients.AccessToken;
using VPBase.Auth.Client.Code.InMemory;
using VPBase.Auth.Client.SharedInterfaces;
using VPBase.Auth.Contract.ConfigEntities;
using VPBase.Auth.Contract.Definitions;
using VPBase.Auth.Contract.Files;
using VPBase.Auth.Contract.SharedInterfaces;
using VPBase.Custom.Tests.Helpers;
using VPBase.Shared.Core.SharedImplementations.AuthContract;
using VPBase.Shared.Core.Types;

namespace VPBase.Custom.Tests.ACC
{
    [TestFixture]
    public  class ACCDependencyTests : MemoryCustomDbTestBase
    {
        [Test]
        public void When_ACC_dependency_resolve()
        {
            // *** Base ***//
            Assert.That(ACC_Base.AppConfigDefinition, Is.Not.Null);
            Assert.That(ACC_Base.TenantConfig, Is.Not.Null);
            Assert.That(ACC_Base.TenantConfigDefinition, Is.Not.Null);
            Assert.That(ACC_Base.ClientCommunicationType, Is.EqualTo(ACCCommunicationType.Test));


            // *** SharedContract - Auth.Contract ***//
            Assert.That(ACC_SharedContract.AuthContractAssemblyHandler, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthContractAssemblyHandler>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthContractCryptionHandler, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthContractCryptionHandler>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthContractDateTimeProvider, Is.Not.Null);
            Assert.That(ACC_SharedContract.TestAuthContractDateTimeProvider, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthContractDateTimeProvider>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<TestAuthContractDateTimeProvider>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthContractFileHandler, Is.Not.Null);
            Assert.That(ACC_SharedContract.TestAuthContractFileHandler, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthContractFileHandler>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<TestAuthContractFileHandler>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthContractJsonConverter, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthContractJsonConverter>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthContractLogger, Is.Not.Null);
            Assert.That(ACC_SharedContract.TestAuthContractLogger, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthContractLogger>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<TestAuthContractLogger>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthContractThreading, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthContractLogger>(), Is.Not.Null);

            // *** SharedContract - Auth.Client ***//
            Assert.That(ACC_SharedContract.AccessTokenSettings, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<AccessTokenSettings>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthClientAccessTokenManager, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthClientAccessTokenManager>(), Is.Not.Null);

            Assert.That(ACC_SharedContract.AuthClientHelper, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IAuthClientHelper>(), Is.Not.Null);

            // ***  CommunicationService *** //
            Assert.That(ACC_CommunicationService.ClientAuthService, Is.Not.Null);
            Assert.That(ACC_CommunicationService.InMemoryClientAuthService, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<IClientAuthService>(), Is.Not.Null);
            Assert.That(ServiceProvider.GetService<InMemoryClientAuthService>(), Is.Not.Null);

            // *** Other ***//
            Assert.That(ACC_Other.ConfigConverter, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<ConfigConverter>(), Is.Not.Null);

            Assert.That(ACC_Other.ConfigFolderFileHandlerSettings, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<ConfigFolderFileHandlerSettings>(), Is.Not.Null);

            Assert.That(ACC_Other.ConfigHandler, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<ConfigHandler>(), Is.Not.Null);

            Assert.That(ACC_Other.TenantInfoHelper, Is.Not.Null);
            Assert.That(ServiceProvider.GetService<TenantInfoHelper>(), Is.Not.Null);
        }
    }
}
