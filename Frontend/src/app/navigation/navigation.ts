import { FuseNavigation } from '@fuse/types';

export const navigation: FuseNavigation[] = [
    {
        'id'      : 'modulos',
        'title'   : 'MODULOS',
        'type'    : 'group',
        'icon'    : 'dashboard',
        'children': [
            {
                'id'        : 'comercial',
                'title'     : 'Comercial',
                'icon' : 'dashboard',
                'type' : 'item',
                'url'  : '/pages/comercial'
            }
        ]
    }
];

export const navigationComercial: FuseNavigation[] = [
    {
        'id'   : 'home',
        'title': 'Início',
        'type' : 'item',
        'icon' : 'home',
        'url'  : '/home'/*,
        'badge': {
            'title': 25,
            'bg'   : '#F44336',
            'fg'   : '#FFFFFF'
        }*/
    },
    /*{
        'id'   : 'modulos',
        'title': 'Comercial Dashboard',
        'type' : 'item',
        'icon' : 'dashboard',
        'url'  : '/pages/comercial'
    },//*/
    {
        'id'      : 'comercial',
        'title'   : 'COMERCIAL',
        'type'    : 'group',
        'icon'    : 'dashboard',
        'children': [
            {
                'id'        : 'agenda',
                'title'     : 'Agenda',
                'type'      : 'item',
                'icon'      : 'calendar_today',
                'url'       : '/pages/comercial/agenda'
            },
            {
                'id'        : 'cadastros',
                'title'     : 'Cadastros',
                'type'      : 'collapsable',
                'icon'      : 'assignment',
                'children'  : [
                    {
                        'id'        : 'geral',
                        'title'     : 'Geral',
                        'type'      : 'collapsable',
                        'icon'      : 'apps',
                        'children'  : [{
                                'id'   : 'demais-servicos',
                                'title': 'Demais Serviços',
                                'type' : 'item',
                                'url'  : '/pages/comercial/cadastros/demais-servicos'
                            },
                            {
                                'id'   : 'condicao-pagamento',
                                'title': 'Condição Pagamento',
                                'type' : 'item',
                                'url'  : '/pages/comercial/cadastros/condicao-pagamento'
                            },
                            {
                                'id'   : 'grupo-economico',
                                'title': 'Grupo Econômico',
                                'type' : 'item',
                                'url'  : '/pages/comercial/cadastros/grupos-economicos'
                            },
                            {
                                'id'   : 'motivo-perda',
                                'title': 'Motivo da Perda',
                                'type' : 'item',
                                'url'  : '/pages/comercial/cadastros/motivos-perda'
                            }
                        ]
                    },
                    {
                        'id'        : 'visitas',
                        'title'     : 'Visitas',
                        'type'      : 'collapsable',
                        'icon'      : 'map',
                        'children'  : [{
                                'id'   : 'visita-tipo',
                                'title': 'Tipos de Visita',
                                'type' : 'item',
                                'url'  : '/pages/comercial/cadastros/tipos-visita'
                            }
                        ]
                    },
                    {
                        'id'        : 'oportunidades',
                        'title'     : 'Oportunidades',
                        'type'      : 'collapsable',
                        'icon'      : 'groups',
                        'children'  : [{
                                'id'   : 'oportunidade-tipo',
                                'title': 'Tipos de Oportunidade',
                                'type' : 'item',
                                'url'  : '/pages/comercial/cadastros/tipos-oportunidade'
                            },
                            {
    
                                'id'   : 'concorrentes',
                                'title': 'Concorrentes',
                                'type' : 'item',
                                'url'  : '/pages/comercial/cadastros/concorrentes'
                            }
                        ]
                    },
                    {
                        'id'        : 'tabela-venda-reajuste',
                        'title'     : 'Tabela de Venda e Reajuste',
                        'type'      : 'collapsable',
                        'icon'      : 'equalizer',
                        'children'  : [{
                            'id'   : 'custo-servico-markup',
                            'title': 'Custo Serviço/Markup',
                            'type' : 'item',
                            'url'  : '/pages/comercial/cadastros/custo-servico-markup'
                        },
                        {
                            'id'   : 'tabela-venda',
                            'title': 'Tabela de Venda',
                            'type' : 'item',
                            'url'  : '/pages/comercial/cadastros/tabela-venda'
                        },
                        {
                            'id'   : 'tabela-venda-aprovacao',
                            'title': 'Aprovação Tabela de Venda',
                            'type' : 'item',
                            'url'  : 'pages/comercial/aprovacoes/tabela-venda'
                        },
                        {
                            'id'   : 'contrato-reajuste-aprovacao',
                            'title': 'Confirma reajuste contrato',
                            'type' : 'item',
                            'url'  : 'pages/comercial/aprovacoes/contrato-reajuste'
                        }
                        ]
                    }
                ]
            },
            {
                'id'        : 'visita',
                'title'     : 'Visita',
                'type'      : 'collapsable',
                'icon'      : 'map',
                'children'  : [
                    {
                        'id'   : 'visita-novo',
                        'title': 'Novo',
                        'type' : 'item',
                        'url'  : '/pages/comercial/visita/nova'
                    },
                    {
                        'id'   : 'visita-lista',
                        'title': 'Lista',
                        'type' : 'item',
                        'url'  : '/pages/comercial/visita/lista'
                    }
                ]
            },
            {
                'id'        : 'lead',
                'title'     : 'Lead',
                'type'      : 'collapsable',
                'icon'      : 'track_changes',
                'children'  : [
                    {
                        'id'   : 'lead-novo',
                        'title': 'Novo',
                        'type' : 'item',
                        'url'  : '/pages/comercial/lead/novo'
                    },
                    {
                        'id'   : 'lead-lista',
                        'title': 'Lista',
                        'type' : 'item',
                        'url'  : '/pages/comercial/lead/lista'
                    }
                ]
            },
            {
                'id'        : 'oportunidade',
                'title'     : 'Oportunidade',
                'type'      : 'collapsable',
                'icon'      : 'groups',
                'children'  : [
                    {
                        'id'   : 'oportunidade-novo',
                        'title': 'Novo',
                        'type' : 'item',
                        'url'  : '/pages/comercial/oportunidade/nova'
                    },
                    {
                        'id'   : 'oportunidade-lista',
                        'title': 'Lista',
                        'type' : 'item',
                        'url'  : '/pages/comercial/oportunidade/lista'
                    }
                ]
            },
            {
                'id'        : 'proposta',
                'title'     : 'Proposta',
                'type'      : 'collapsable',
                'icon'      : 'question_answer',
                'children'  : [
                    {
                        'id'   : 'proposta-nova',
                        'title': 'Nova',
                        'type' : 'item',
                        'url'  : '/pages/comercial/proposta/nova'
                    },
                    {
                        'id'   : 'proposta-lista',
                        'title': 'Lista',
                        'type' : 'item',
                        'url'  : '/pages/comercial/proposta/lista'
                    }
                ]
            },
            {
                'id'        : 'contrato',
                'title'     : 'Contrato',
                'type'      : 'collapsable',
                'icon'      : 'description',
                'children'  : [
                    {
                        'id'   : 'contrato-lista',
                        'title': 'Lista',
                        'type' : 'item',
                        'url'  : '/pages/comercial/contrato/lista'
                    },
                    {
                        'id'   : 'carteiro-page',
                        'title': 'Carteira',
                        'type' : 'item',
                        'url'  : '/pages/comercial/contrato/carteira'
                    },
                    {
                        'id'   : 'solicita-pagamento-cartao',
                        'title': 'Solicita Pagamento',
                        'type' : 'item',
                        'url'  : '/pages/comercial/pagamentos/solicitacao'
                    }
                ]
            },
            {
                'id'        : 'aprovacoes',
                'title'     : 'Aprovações',
                'type'      : 'collapsable',
                'icon'      : 'thumb_up',
                'children'  : [
                    {
                        'id'   : 'proposta-aprova',
                        'title': 'Lista',
                        'type' : 'item',
                        'url'  : '/pages/comercial/proposta/aprovacoes'
                    }
                ]
            },
            {
                'id'        : 'programacao',
                'title'     : 'Programação',
                'type'      : 'collapsable',
                'icon'      : 'date_range',
                'children'  : [
                    {
                        'id'   : 'programacao-lista',
                        'title': 'Lista',
                        'type' : 'item',
                        'url'  : '/pages/comercial/programacao/lista'
                    }
                ]
            },
            {
                'id'        : 'relatorios',
                'title'     : 'Relatórios',
                'type'      : 'collapsable',
                'icon'      : 'insert_chart',
                'children'  : [
                    {
                        'id'   : 'relatorio-producao',
                        'title': 'Relatório de produção',
                        'type' : 'item',
                        'url'  : '/pages/comercial/relatorios/producao'
                    }
                ]
            },
            {
                'id'        : 'configuracoes',
                'title'     : 'Configurações',
                'type'      : 'collapsable',
                'icon'      : 'settings',
                'children'  : [
                    {
                        'id'   : 'assinatura-eletronica',
                        'title': 'Configurar Assinatura Eletrônica',
                        'type' : 'item',
                        'url'  : '/pages/comercial/configuracoes/assinatura-eletronica'
                    },
                    {
                        'id'   : 'versionamento-contrato',
                        'title': 'Versionamento de Contrato',
                        'type' : 'item',
                        'url'  : '/pages/comercial/configuracoes/versionamento-contrato'
                    },
                    {
                        'id'   : 'aprovacao-comercial-configuracao',
                        'title': 'Aprovação Comercial',
                        'type' : 'item',
                        'url'  : 'pages/comercial/configuracoes/aprovacao-comercial'
                    },
                    {
                        'id'   : 'acesso-aprovacoes-configuracao',
                        'title': 'Configurar Acesso Aprovações',
                        'type' : 'item',
                        'url'  : 'pages/comercial/configuracoes/acesso-aprovacoes'
                    }
                ]
            }
        ]
    }
];
