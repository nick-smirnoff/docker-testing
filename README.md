# docker-testing

```shell
docker compose --file build/docker-compose/docker-compose.tests.yml up --build
```

---

```shell
# All tests
docker build --file build/docker/Dockerfile.tests --tag docker-testing-demo-tests .
# Filter to only run unit tests
docker build --file build/docker/Dockerfile.tests --tag docker-testing-demo-tests --target unit-test-runner .
# Filter to only run component tests
docker build --file build/docker/Dockerfile.tests --tag docker-testing-demo-tests --target component-test-runner .

docker run --rm --volume "$(pwd)/.results:/.results" docker-testing-demo-tests
```