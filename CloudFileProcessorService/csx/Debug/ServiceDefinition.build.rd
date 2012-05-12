<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="CloudFileProcessorService" generation="1" functional="0" release="0" Id="316d87fa-eb71-46af-8500-344e292ff90a" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="CloudFileProcessorServiceGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Processor_WebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/LB:Processor_WebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Processor_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/MapProcessor_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Processor_WebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/MapProcessor_WebRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Processor_WebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/Processor_WebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapProcessor_WebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/Processor_WebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapProcessor_WebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/Processor_WebRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Processor_WebRole" generation="1" functional="0" release="0" software="C:\Users\Tarwn\Documents\Visual Studio 2010\Projects\CloudFileProcessorService\CloudFileProcessorService\csx\Debug\roles\Processor_WebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Processor_WebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Processor_WebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/Processor_WebRoleInstances" />
            <sCSPolicyFaultDomainMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/Processor_WebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="Processor_WebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Processor_WebRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="91b08a4c-1f24-4d4a-8f84-aac9a101948a" ref="Microsoft.RedDog.Contract\ServiceContract\CloudFileProcessorServiceContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="6509fa38-dac2-4392-a7a3-bcf068751a6d" ref="Microsoft.RedDog.Contract\Interface\Processor_WebRole:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/CloudFileProcessorService/CloudFileProcessorServiceGroup/Processor_WebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>