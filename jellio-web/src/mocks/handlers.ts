import { http, HttpResponse } from 'msw';

export const handlers = [
  http.get('/jelliodirect/server-info', () =>
    HttpResponse.json({
      name: 'Dev Jellyfin',
      libraries: [
        {
          Name: 'Movies',
          Id: '00000000-0000-0000-0000-000000000001',
          CollectionType: 'movies',
        },
        {
          Name: 'TV Shows',
          Id: '00000000-0000-0000-0000-000000000002',
          CollectionType: 'tvshows',
        },
      ],
    }),
  ),
  http.get('/jelliodirect/get-config', () =>
    HttpResponse.json({
      jellyseerrEnabled: false,
      jellyseerrUrl: '',
      jellyseerrApiKey: '',
      publicBaseUrl: '',
      selectedLibraries: [],
    }),
  ),
  http.post('/jelliodirect/start-session', () =>
    HttpResponse.json({ accessToken: 'dev-addon-session-token' }),
  ),
  http.post('/jelliodirect/save-config', () => HttpResponse.json({ ok: true })),
  http.get('/jelliodirect/logs', () =>
    HttpResponse.json({
      logs: [
        {
          timestamp: new Date().toISOString(),
          message: 'Dev MSW: stub log entry',
          level: 'Info',
        },
      ],
    }),
  ),
  http.post('/jelliodirect/logs/clear', () => HttpResponse.json({ ok: true })),
];
