import http from 'k6/http';
import { check } from 'k6';

export const options = {
  scenarios: {
    fixed_rate_test: {
      executor: 'constant-arrival-rate',
      rate: 30, // 50 iterations per second
      timeUnit: '1s', // per second
      duration: '60s', // test duration
      preAllocatedVUs: 100, // initial VUs to allocate
      maxVUs: 200, // max VUs K6 can scale up to
    },
  },
};

const BASE_URL = 'http://localhost:18081'; // substitua aqui se necessário
const TOKEN = 'eyJhbGciOiJSUzI1NiIsImtpZCI6IjI5NDBiZmI4LTcwNTYtNDE4Zi04ZWM4LTlhNGMwYmJlNjhiZSIsInR5cCI6IkpXVCJ9.eyJuYW1lIjoiVEVTVEFORE8gSldUIiwic3ViIjoiNGViMDVlNTUtM2IxYS00NDU3LWI1OTgtMzIwYmNiNzQyM2NiIiwiaWF0IjoxNzQ1MDA2OTQyLCJleHAiOjE3NDUwMTA1NDIsImlzcyI6Imh0dHBzOi8vaWRlbnRpdHktc2VydmVyLWRlYnVnLmNvbSIsImF1ZCI6ImF1ZC1pZGVudGl0eS1zZXJ2ZXItZGVidWcifQ.f_zXEWpuFqfLwbbWsVpxV77clbATigFmoL_XUuPG59Zk2ZBqSXM4znZ16vtC9kLls9piWN0jD1jwXlyGIGWL3eWHtBah3FNPTaDcTN_0HWjKleB74Vj1iYtK7bEcVXR26Olg6VsfWrWzsp4hn1xQQZVJrIWPHemDcnsvC9sCBSvvxuVem-oNSa3wEC53MFhSh4puiyJOOSO5Q4ES0b7GKHy0msKD7iigK2O9OtX1ZxS83JjERv6Fo2_2hFcTPu6UEEV75-NWK9yg1po1WkbaTzEs-y0RWoAnsTIwthDCG3ynE6X_UwPj912DAGkQJxZcvDV4Va5ULm72AfNaotYDMQ';

export default function () {
  const url = `${BASE_URL}/api/v1/lancamentos`;
  const payload = JSON.stringify({
    comercianteId: 'c0d84b50-b859-4c21-b18d-1612a8bdb0a3',
    isCredito: true,
    valor: 1,
  });

  const headers = {
    'Content-Type': 'application/json',
    'Authorization': `Bearer ${TOKEN}`,
  };

  const res = http.post(url, payload, { headers });

  check(res, {
    'status is 200': (r) => r.status === 200,
    // 'status is 201': (r) => r.status === 201, // caso a API retorne 201 ao criar
  });
}
