using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Structurizr.Api;

namespace Structurizr
{
    public class StructurizrWorkspace
    {
        private long workspaceId;
        private string apiKey;
        private string apiSecret;
        
        private readonly Workspace workspace;

        private Person studentDesk;
        private Person student;
        private SoftwareSystem system;
        private SoftwareSystem xlsx;

        private Container frontend;
        private Container backend;
        private Container database;
        private Container importer;

        private Component api;
        private Component application;
        private Component domain;
        private Component persistence;
        private Component identityService;


        public StructurizrWorkspace(string apiKey, string apiSecret, long workspaceId)
        {
            this.apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));
            this.apiSecret = apiSecret ?? throw new ArgumentNullException(nameof(apiSecret));
            this.workspaceId = workspaceId > 0 ? workspaceId : throw new ArgumentOutOfRangeException(nameof(workspaceId));

            workspace = new Workspace("SIS",
                "Student Information System");

            DefineContextView();
            DefineContainerView();
            DefineComponentsView();
            DefineStyles();
        }

        private void DefineContextView()
        {
            var model = workspace.Model;

            studentDesk = model.AddPerson("Student Desk");
            student = model.AddPerson("Student");
            system = model.AddSoftwareSystem("Student Information System (SIS)");
            
            studentDesk.Uses(system, "Uses");
            student.Uses(system, "Uses");

            var contextView = workspace.Views.CreateSystemContextView(system, "System Context", "System Context diagram.");
            contextView.AddAllElements();
            contextView.EnableAutomaticLayout();
        }

        private void DefineContainerView()
        {
            frontend = system.AddContainer("Angular Frontend");
            backend = system.AddContainer("Dotnet Backend");
            database = system.AddContainer("Database");
            database.AddTags("database");

            frontend.Uses(backend, "Uses");
            backend.Uses(database, "Uses");
            studentDesk.Uses(frontend, "Uses");
            student.Uses(frontend, "Uses");

            var containerView = workspace.Views.CreateContainerView(system, "Containers", "Container Diagram");
            containerView.AddAllElements();
            containerView.EnableAutomaticLayout();
        }

        private void DefineComponentsView()
        {
            api = backend.AddComponent("API");
            application = backend.AddComponent("Core.Application");
            domain = backend.AddComponent("Core.Domain");
            persistence = backend.AddComponent("Persistence");
            identityService = backend.AddComponent("IdentityService");

            api.Uses(application, "Uses");
            application.Uses(domain, "Uses");
            persistence.Uses(domain, "Uses");
            persistence.Uses(application, "Uses");
            identityService.Uses(application, "Uses");
            identityService.Uses(domain, "Uses");
            identityService.Uses(persistence, "Uses");

            frontend.Uses(api, "Uses");
            persistence.Uses(database, "Uses");
            identityService.Uses(database, "Uses");

            var componentView = workspace.Views.CreateComponentView(backend, "Components", "Components Diagram");
            componentView.AddDefaultElements();
            //componentView.EnableAutomaticLayout();
        }

        private void DefineStyles()
        {
            var styles = workspace.Views.Configuration.Styles;

            styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });
            styles.Add(new ElementStyle("file") { Background = "#D3D3D3", Color = "#000000" });
            styles.Add(new ElementStyle(Tags.Container) { Background = "#1168bd", Color = "#ffffff" });
            styles.Add(new ElementStyle("database") { Background = "#1168bd", Color = "#ffffff", Shape = Shape.Cylinder });
            styles.Add(new ElementStyle(Tags.Component) { Background = "#1168bd", Color = "#ffffff" });
        }

        public void PutWorkspace()
        {
            var structurizrClient = new StructurizrClient(apiKey, apiSecret);
            structurizrClient.PutWorkspace(workspaceId, workspace);
        }
    }
}
