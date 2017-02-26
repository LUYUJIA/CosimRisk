Ext.define( 'CosimRisk.controller.Main', {
    extend: 'Ext.app.Controller',
    views: ['CosimRisk.view.ViewPanel'],
    init: function () {
        Ext.create( 'CosimRisk.view.ViewPanel' );
    }
} );