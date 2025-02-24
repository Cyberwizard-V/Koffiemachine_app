import http from 'k6/http';
import { check, group } from 'k6';
import { sleep } from 'k6';
import { SharedArray } from 'k6/data';

const BASE_URL = 'https://localhost:7075/api/machines';

//totaal 2 minuten test
export let options = {
    stages: [
        { duration: '30s', target: 10 },
        { duration: '1m', target: 100 },
        { duration: '30s', target: 0 },
    ],
};

export default function () {
    group('Get Machines', function () {
        const res = http.get(`${BASE_URL}?page=1&pageSize=10`);
        check(res, {
            'is status 200': (r) => r.status === 200,
            'response time < 500ms': (r) => r.timings.duration < 500,
        });
        sleep(1);
    });
}
